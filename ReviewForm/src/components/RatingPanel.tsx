import {
  useState,
  useEffect,
  useRef,
  useMemo,
  useId,
  useCallback,
} from "react";
import { AngryFace } from "./icons/AngryFace.tsx";
import { FrowningFace } from "./icons/FrowningFace.tsx";
import { GrinningFace } from "./icons/GrinningFace.tsx";
import { NeutralFace } from "./icons/NeutralFace.tsx";
import { SmilingFace } from "./icons/SmilingFace.tsx";

interface RatingPanelProps {
  children: string;
  value: number;
  onChange: (rating: number) => void;
  name: string;
  id?: string;
}

const COLORS = ["#F24E1E", "#FF8311", "#FF8311", "#FFC700", "#FFC700"];
const MAX_RATING = 4;
const MIN_RATING = 0;

export const RatingPanel = ({
  children,
  value: currentValue,
  onChange,
  name,
  id: propId,
}: RatingPanelProps) => {
  const generatedId = useId();
  const id = propId || generatedId;
  const [isDragging, setIsDragging] = useState(false);
  const containerRef = useRef<HTMLDivElement>(null);

  const emojis = useMemo(
    () => [
      <AngryFace key="angry" />,
      <FrowningFace key="frowning" />,
      <NeutralFace key="neutral" />,
      <SmilingFace key="smiling" />,
      <GrinningFace key="grinning" />,
    ],
    []
  );

  const isSelected = currentValue >= MIN_RATING && currentValue <= MAX_RATING;
  const normalizedValue = isSelected ? currentValue : -1;

  const handleRadioClick = useCallback(
    (rating: number) => onChange(rating),
    [onChange]
  );

  const handleMouseDown = useCallback(
    (rating: number) => {
      setIsDragging(true);
      onChange(rating);
    },
    [onChange]
  );

  const calculateRatingFromMousePosition = useCallback((clientX: number) => {
    if (!containerRef.current) return -1;
    const { left, width } = containerRef.current.getBoundingClientRect();
    const offsetX = clientX - left;
    const buttonWidth = width / 5;
    return Math.floor(offsetX / buttonWidth);
  }, []);

  useEffect(() => {
    const handleMouseMove = (e: MouseEvent) => {
      if (!isDragging) return;
      const newRating = calculateRatingFromMousePosition(e.clientX);
      if (newRating !== -1 && newRating !== normalizedValue) {
        onChange(newRating);
      }
    };

    const handleMouseUp = () => setIsDragging(false);

    window.addEventListener("mousemove", handleMouseMove);
    window.addEventListener("mouseup", handleMouseUp);

    return () => {
      window.removeEventListener("mousemove", handleMouseMove);
      window.removeEventListener("mouseup", handleMouseUp);
    };
  }, [isDragging, normalizedValue, onChange, calculateRatingFromMousePosition]);

  return (
    <div className="d-flex flex-row gap-3 align-items-center">
      <div className="d-flex w-75 position-relative" ref={containerRef}>
        <div
          className="d-flex progress position-absolute start-50 top-50 translate-middle w-100"
          style={{
            height: "10px",
            border: "1px solid #DEDEDE",
            background: "transparent",
          }}
        >
          <div
            className="d-flex progressbar position-absolute"
            style={{
              height: "10px",
              width: isSelected ? `${normalizedValue * 25}%` : "0%",
              backgroundColor: isSelected
                ? COLORS[normalizedValue]
                : "transparent",
            }}
          />
        </div>
        <input
          className="visually-hidden"
          type="range"
          min={MIN_RATING}
          max={MAX_RATING}
          value={isSelected ? normalizedValue : MIN_RATING}
          name={name}
          id={`input-item-${id}`}
          onChange={(e) => onChange(Number(e.target.value))}
        />
        <div className="d-flex justify-content-between position-absolute start-50 top-50 translate-middle w-100">
          {[0, 1, 2, 3, 4].map((rating) => (
            <div
              key={rating}
              className="position-relative rounded-circle"
              style={{
                width: "14px",
                height: "14px",
                border: "1px solid #DEDEDE",
                cursor: "pointer",
                backgroundColor:
                  isSelected && rating <= normalizedValue
                    ? COLORS[normalizedValue]
                    : "#FFFFFF",
              }}
              onClick={() => handleRadioClick(rating)}
              onMouseDown={(e) => {
                e.preventDefault();
                handleMouseDown(rating);
              }}
              role="slider"
              aria-valuenow={normalizedValue}
              aria-valuemin={MIN_RATING}
              aria-valuemax={MAX_RATING}
            >
              {isSelected && normalizedValue === rating && (
                <div
                  className="position-absolute top-0 start-50 translate-middle"
                  style={{
                    width: "16px",
                    height: "16px",
                  }}
                >
                  {emojis[normalizedValue]}
                </div>
              )}
            </div>
          ))}
        </div>
      </div>
      <label
        className="w-25 text-nowrap text-start"
        htmlFor={`input-item-${id}`}
      >
        {children}
      </label>
    </div>
  );
};
