import React, { memo } from "react";

const InsertPreview: React.FC = memo(() => {
  return (
    <div
      className="my-2 rounded-2"
      style={{
        border: "2px dotted #0d6efd",
        borderRadius: "4px",
        height: "60px",
      }}
    ></div>
  );
});

export default InsertPreview;
