import React, { memo } from "react";

const InsertPreview: React.FC = memo(() => {
  return (
    <div
      className="my-2 rounded-2"
      style={{
        border: "3px dotted #0d6efd",
        backgroundColor: "rgb(112, 169, 255)",
        borderRadius: "4px",
        height: "60px",
        opacity: "30%",
      }}
    ></div>
  );
});

export default InsertPreview;
