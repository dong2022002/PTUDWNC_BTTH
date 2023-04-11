import React, { useEffect, useState } from "react";
import { ListGroup } from "react-bootstrap";
import { Link } from "react-router-dom";
import { getArchivesPosts } from "../Services/BlogRepository";

const ArchivesWidget = () => {
  const [postList, setPostList] = useState([]);
  const monthNames = ["January", "February", "March", "April", "May", "June",
  "July", "August", "September", "October", "November", "December"
  ];
  useEffect(() => {
    getArchivesPosts(12).then((data) => {
      if (data) setPostList(data);
      else setPostList([]);
    });
  }, []);
  return (
    <div className="mb-4">
      <h3 className="text-success mb-2">Bài viết trong 12 tháng gần nhất </h3>
      {postList.length > 0 && (
        <ListGroup>
          {postList.map((item, index) => {
            return (
              <ListGroup.Item key={index}>
                <Link
                  to={`/blog/post?month=${item.month}&year=${item.year}`}
                  title={item.month}
                  key={index}
                >
                  {monthNames[item.month-1]}<span> &nbsp;({item.postCount}) </span>
                </Link>
              </ListGroup.Item>
            );
          })}
        </ListGroup>
      )}
    </div>
  );
};

export default ArchivesWidget
