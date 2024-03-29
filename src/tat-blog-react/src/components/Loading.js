import React from "react";
import { Button, Spinner } from "react-bootstrap";
const Loading = () => {
  return (
    <div className="text-center">
      <Button variant="outline-success" disabled style={{ boder: "none" }}>
        <Spinner
          as="span"
          animation="grow"
          size="sm"
          role="status"
          aria-hidden="true"
        />
        &nbsp;Đang tải ...
      </Button>
    </div>
  );
};

export default Loading;
