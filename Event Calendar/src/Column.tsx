import React, { memo } from "react";
import CardComponent from "./Card";
import InsertPreview from "./InsertPreview";

type CardType = {
  id: string;
  content: string;
  order: number;
};

type ColumnType = {
  id: string;
  title: string;
  cards: CardType[];
};

type ColumnProps = {
  column: ColumnType;
  onCardMouseDown: (
    colId: string,
    cardId: string,
    clientX: number,
    clientY: number,
    offsetX: number,
    offsetY: number
  ) => void;
  dropIndex: number | null;
  draggingCardId: string | null;
  columnRef: (el: HTMLDivElement | null) => void;
  cardRefs: React.MutableRefObject<{ [key: string]: HTMLDivElement | null }>;
};

const Column: React.FC<ColumnProps> = memo(
  ({
    column,
    onCardMouseDown,
    dropIndex,
    draggingCardId,
    columnRef,
    cardRefs,
  }) => {
    return (
      <div className="col" ref={columnRef}>
        <div className="card p-2 mb-4">
          <div className="card-body">
            <h5 className="card-title">{column.title}</h5>
            {dropIndex === 0 && <InsertPreview />}
            {column.cards.map((card, index) => (
              <React.Fragment key={card.id}>
                <CardComponent
                  card={card}
                  columnId={column.id}
                  onDragStart={onCardMouseDown}
                  isDragging={draggingCardId === card.id}
                  cardRef={(el: HTMLDivElement | null) => {
                    cardRefs.current[card.id] = el;
                  }}
                />
                {dropIndex === index + 1 && <InsertPreview />}
              </React.Fragment>
            ))}
            {/* {dropIndex === column.cards.length && <InsertPreview />} */}
          </div>
        </div>
      </div>
    );
  }
);

export default Column;
