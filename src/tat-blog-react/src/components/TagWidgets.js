import React, { useEffect, useState } from "react";
import { getTags } from "../Services/TagRepository";
import TagList from "./TagList";

const TagWidgets = () => {
  const [tagLists, setTagLists] = useState([]);
  useEffect(() => {
    document.title = "Trang chủ";

    getTags().then((data) => {
      if (data) {
        setTagLists(data.items);
      } else setTagLists([]);
    });
  }, []);
  return (
    <TagList tagList={tagLists}/>
    )
};

export default TagWidgets;
