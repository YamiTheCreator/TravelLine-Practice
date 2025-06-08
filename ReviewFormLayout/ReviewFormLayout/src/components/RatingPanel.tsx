import { useCallback } from "react";
import { AngryFace } from "./icons/AngryFace.tsx";
import { FrowningFace } from "./icons/FrowningFace.tsx";
import { NeutralFace } from "./icons/NeutralFace.tsx";
import { SmilingFace } from "./icons/SmilingFace.tsx";
import { GrinningFace } from "./icons/GrinningFace.tsx";

interface RatingPanelProps {
  value: number;
  onChange: (rating: number) => void;
}

const ratingIcons = [
  { icon: AngryFace, value: 0 },
  { icon: FrowningFace, value: 1 },
  { icon: NeutralFace, value: 2 },
  { icon: SmilingFace, value: 3 },
  { icon: GrinningFace, value: 4 },
];

export const RatingPanel = ({
  value: currentValue,
  onChange,
}: RatingPanelProps) => {
  const handleRatingClick = useCallback(
    (rating: number) => {
      onChange(rating);
    },
    [onChange]
  );

  return (
    <div className="d-flex flex-row align-items-center justify-content-between">
      {ratingIcons.map(({ icon: IconComponent, value }) => (
        <button
          key={value}
          type="button"
          onClick={() => handleRatingClick(value)}
          className={`btn px-4 border rounded-5 ${
            currentValue === value ? "bg-primary text-white" : "bg-white"
          }`}
          style={{ minWidth: "48px", minHeight: "48px" }}
        >
          <IconComponent />
        </button>
      ))}
    </div>
  );
};
