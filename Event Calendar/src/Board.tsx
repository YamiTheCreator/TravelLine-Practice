import React, { useState, useRef, useCallback, useEffect } from "react";
import Column from "./Column";
import DragPreview from "./DragPreview";

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

type DragState = {
  cardId: string;
  originColumnId: string;
  originIndex: number;
  offsetX: number;
  offsetY: number;
  cardData: CardType;
};

const Board: React.FC = () => {
  const initialData: ColumnType[] = [
    {
      id: "col1",
      title: "Понедельник",
      cards: [
        { id: "card1", content: "Интересное дело", order: 1 },
        { id: "card2", content: "Скучное дело", order: 2 },
        { id: "card3", content: "Полезное дело", order: 3 },
      ],
    },
    {
      id: "col2",
      title: "Вторник",
      cards: [
        { id: "card4", content: "Важное дело", order: 1 },
        { id: "card5", content: "Нужное дело", order: 2 },
      ],
    },
    {
      id: "col3",
      title: "Среда",
      cards: [],
    },
    {
      id: "col4",
      title: "Четверг",
      cards: [],
    },
  ];
  const [columns, setColumns] = useState<ColumnType[]>(initialData);

  const [dragState, setDragState] = useState<DragState | null>(null);

  const [previewPos, setPreviewPos] = useState<{ x: number; y: number }>({
    x: 0,
    y: 0,
  });

  const [dropTarget, setDropTarget] = useState<{
    colId: string;
    index: number;
  } | null>(null);

  const columnRefs = useRef<{ [key: string]: HTMLDivElement | null }>({});
  const cardRefs = useRef<{ [key: string]: HTMLDivElement | null }>({});

  const handleDragStart = useCallback(
    (
      colId: string,
      cardId: string,
      clientX: number,
      clientY: number,
      offsetX: number,
      offsetY: number
    ) => {
      const column = columns.find((col) => col.id === colId);
      if (!column) return;
      const originIndex = column.cards.findIndex((card) => card.id === cardId);
      if (originIndex === -1) return;

      const cardData = column.cards[originIndex];

      setDragState({
        cardId,
        originColumnId: colId,
        originIndex,
        offsetX,
        offsetY,
        cardData,
      });

      setPreviewPos({ x: clientX - offsetX, y: clientY - offsetY });
    },
    [columns]
  );

  const handleMouseMove = useCallback(
    (e: MouseEvent) => {
      if (!dragState) return;

      setPreviewPos({
        x: e.clientX - dragState.offsetX,
        y: e.clientY - dragState.offsetY,
      });

      let targetColId: string | null = null;
      columns.forEach((col) => {
        const ref = columnRefs.current[col.id];
        if (!ref) return;
        const rect = ref.getBoundingClientRect();
        if (
          e.clientX >= rect.left &&
          e.clientX <= rect.right &&
          e.clientY >= rect.top &&
          e.clientY <= rect.bottom
        ) {
          targetColId = col.id;
        }
      });

      if (!targetColId) {
        setDropTarget(null);
        return;
      }

      const targetColumn = columns.find((col) => col.id === targetColId)!;
      const cards = targetColumn.cards;
      let insertionIndex = cards.length;
      for (let i = 0; i < cards.length; i++) {
        const card = cards[i];
        if (
          targetColId === dragState.originColumnId &&
          card.id === dragState.cardId
        ) {
          continue;
        }
        const cardElement = cardRefs.current[card.id];
        if (!cardElement) continue;
        const cardRect = cardElement.getBoundingClientRect();
        const cardCenterY = cardRect.top + cardRect.height / 2;
        if (e.clientY < cardCenterY) {
          insertionIndex = i;
          break;
        }
      }

      if (targetColId === dragState.originColumnId) {
        if (
          insertionIndex === dragState.originIndex ||
          insertionIndex === dragState.originIndex + 1
        ) {
          setDropTarget(null);
          return;
        }
      }

      setDropTarget({ colId: targetColId, index: insertionIndex });
    },
    [columns, dragState]
  );

  const handleMouseUp = useCallback(() => {
    if (!dragState) return;
    if (dropTarget) {
      setColumns((prevColumns) => {
        const newColumns = prevColumns.map((col) => ({
          ...col,
          cards: [...col.cards],
        }));
        const originCol = newColumns.find(
          (col) => col.id === dragState.originColumnId
        )!;
        const cardIndex = originCol.cards.findIndex(
          (c) => c.id === dragState.cardId
        );
        const [movedCard] = originCol.cards.splice(cardIndex, 1);
        const targetCol = newColumns.find(
          (col) => col.id === dropTarget.colId
        )!;
        let insertIndex = dropTarget.index;
        if (targetCol.id === originCol.id && cardIndex < insertIndex) {
          insertIndex--;
        }
        targetCol.cards.splice(insertIndex, 0, movedCard);
        newColumns.forEach((col) => {
          col.cards.forEach((card, idx) => {
            card.order = idx + 1;
          });
        });
        return newColumns;
      });
    }
    setDragState(null);
    setDropTarget(null);
  }, [dragState, dropTarget]);

  useEffect(() => {
    if (dragState) {
      window.addEventListener("mousemove", handleMouseMove);
      window.addEventListener("mouseup", handleMouseUp);
    }
    return () => {
      window.removeEventListener("mousemove", handleMouseMove);
      window.removeEventListener("mouseup", handleMouseUp);
    };
  }, [dragState, handleMouseMove, handleMouseUp]);

  return (
    <div className="container mt-4" style={{ position: "relative" }}>
      <div className="row">
        {columns.map((col) => (
          <Column
            key={col.id}
            column={col}
            onCardMouseDown={handleDragStart}
            dropIndex={
              dropTarget && dropTarget.colId === col.id
                ? dropTarget.index
                : null
            }
            draggingCardId={dragState ? dragState.cardId : null}
            columnRef={(el: HTMLDivElement | null) => {
              columnRefs.current[col.id] = el;
            }}
            cardRefs={cardRefs}
          />
        ))}
      </div>

      {dragState && (
        <DragPreview
          x={previewPos.x}
          y={previewPos.y}
          card={dragState.cardData}
        />
      )}
    </div>
  );
};

export default Board;
