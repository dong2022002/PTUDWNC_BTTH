import React, { useEffect, useState } from "react";
import { Table } from "react-bootstrap";
import { useSelector } from "react-redux";
import { Link, useParams } from "react-router-dom";
import PostFilterPane from "../../../components/Admin/PostFilterPane";
import Loading from "../../../components/Loading";
import { getPostFilter } from "../../../Services/BlogRepository";
const Posts = () => {
  const [postsList, setPostsList] = useState([]);
  const [isVisibleLoading, setIsVisibleLoading] = useState(true);
  const postFilter = useSelector((state) => state.postFilter);

  let { id } = useParams();
  let p = 1;
  let ps = 10;

  useEffect(() => {
    document.title = "Danh sách bài viết";

    getPostFilter(
      postFilter.keyword,
      postFilter.authorId,
      postFilter.categoryId,
      postFilter.year,
      postFilter.month,
      ps,
      p
    ).then((data) => {
      if (data) {
        setPostsList(data.items);
      } else {
        setPostsList([]);
      }
      setIsVisibleLoading(false);
    });
  }, [
    postFilter.keyword,
    postFilter.authorId,
    postFilter.categoryId,
    postFilter.year,
    postFilter.month,
    ps,
    p,
  ]);
  return (
    <>
      <h1>Danh sách bài viết {id} </h1>
      <PostFilterPane />
      {isVisibleLoading ? (
        <Loading />
      ) : (
        <Table striped responsive bordered>
          <thead>
            <tr>
              <th>Tiêu đề</th>
              <th>Tác giả</th>
              <th>Chủ đề</th>
              <th>Xuất bản</th>
            </tr>
          </thead>
          <tbody>
            {postsList.length > 0 ? (
              postsList.map((item, index) => (
                <tr key={index}>
                  <td>
                    <Link
                      to={`/admin/post/edit/${item.id}`}
                      className="text-bold"
                    >
                      {item.title}
                    </Link>
                    <p className="text-muted">{item.shortDescription}</p>
                  </td>
                  <td>{item.author.fullName}</td>
                  <td>{item.category.name}</td>
                  <td>{item.published ? "Có" : "Không"}</td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={4}>
                  <h4 className="text-danger text-center">
                    Không tìm thấy bài viết nào
                  </h4>
                </td>
              </tr>
            )}
          </tbody>
        </Table>
      )}
    </>
  );
};

export default Posts;
