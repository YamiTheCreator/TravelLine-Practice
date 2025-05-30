import React, { memo, useCallback, useRef } from "react";

type CardType = {
  id: string;
  content: string;
  order: number;
};

type CardProps = {
  card: CardType;
  columnId: string;
  onDragStart: (
    colId: string,
    cardId: string,
    clientX: number,
    clientY: number,
    offsetX: number,
    offsetY: number
  ) => void;
  isDragging: boolean;
  cardRef: (el: HTMLDivElement | null) => void;
};

const CardComponent: React.FC<CardProps> = memo(
  ({ card, columnId, onDragStart, isDragging, cardRef }) => {
    const cardElementRef = useRef<HTMLDivElement>(null);

    const handleMouseDown = useCallback(
      (e: React.MouseEvent) => {
        e.preventDefault();
        const el = cardElementRef.current;
        if (!el) return;
        const rect = el.getBoundingClientRect();

        const offsetX = e.clientX - rect.left;
        const offsetY = e.clientY - rect.top;
        onDragStart(columnId, card.id, e.clientX, e.clientY, offsetX, offsetY);
      },
      [onDragStart, columnId, card.id]
    );

    return (
      <div
        ref={(el) => {
          cardElementRef.current = el;
          cardRef(el);
        }}
        className="card mb-2"
        style={{
          opacity: isDragging ? 0.5 : 1,
          cursor: "grab",
          border: isDragging ? "3px dotted gray" : "",
          backgroundColor: isDragging ? "#EEEEEE" : "",
        }}
        onMouseDown={handleMouseDown}
      >
        <div className="card-body">
          <p className="card-text">{card.content}</p>
        </div>
      </div>
    );
  }
);

export default CardComponent;
