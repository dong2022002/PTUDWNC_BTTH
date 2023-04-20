import React, { useEffect, useState } from "react";
import { Table } from "react-bootstrap";
import { useSelector } from "react-redux";
import { Link, useParams } from "react-router-dom";
import PostFilterPane from "../../../components/Admin/PostFilterPane";
import Loading from "../../../components/Loading";
import PagerAdmin from "../../../components/PagerAdmin";
import {
  changePublished,
  getPostFilter,
} from "../../../Services/BlogRepository";
import { useQuery } from "../../../Utils/Utils";
const Posts = () => {
  const [postsList, setPostsList] = useState([]);
  const [metadata, setMetadata] = useState([]);
  const [reload, setReload] = useState(false);
  const [isVisibleLoading, setIsVisibleLoading] = useState(true);
  const postFilter = useSelector((state) => state.postFilter);

  let { id } = useParams();
  let query = useQuery();
  let p = query.get("p") ?? 1;
  let ps = query.get("ps") ?? 10;

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
        setMetadata(data.metadata);
      } else {
        setPostsList([]);
      }
      setIsVisibleLoading(false);
    });
    setReload(false);
    console.log(reload);
  }, [
    postFilter.keyword,
    postFilter.authorId,
    postFilter.categoryId,
    postFilter.year,
    postFilter.month,
    ps,
    p,
    reload,
  ]);
  function onchangePublished(e) {
    changePublished(e.target.value).then(() => {
      console.log("setReload");
      setReload(true);
    });
  }
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
                      to={`/admin/posts/edit/${item.id}`}
                      className="text-bold"
                    >
                      {item.title}
                    </Link>
                    <p className="text-muted">{item.shortDescription}</p>
                  </td>
                  <td>{item.author.fullName}</td>
                  <td>{item.category.name}</td>
                  <td>
                    <button
                      class={
                        item.published ? "btn btn-primary" : "btn btn-danger"
                      }
                      onClick={onchangePublished}
                      value={item.id}
                    >
                      {item.published ? "Có" : "Không"}
                    </button>
                  </td>
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
            <div className="p-4 ">
              <PagerAdmin metadata={metadata} controller={"/admin/posts"} />
            </div>
          </tbody>
        </Table>
      )}
    </>
  );
};

export default Posts;
