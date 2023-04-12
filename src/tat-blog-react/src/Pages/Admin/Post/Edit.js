import React, { useEffect, useState } from "react";
import { Button, Form } from "react-bootstrap";
import { Link, Navigate, useParams } from "react-router-dom";
import {
  addOrUpdatePost,
  getFilter,
  getPostById,
} from "../../../Services/BlogRepository";
import { decode, isEmptyOrSpaces, isInteger } from "../../../Utils/Utils";
const Edit = () => {
  const initialState = {
    id: 0,
    title: "",
    shortDescription: "",
    description: "",
    urlSlug: "",
    meta: "",
    imageUrl: "",
    ImageFile: null,
    category: {},
    author: {},
    tags: {},
    selectedTags: "",
    published: false,
  };
  const [post, setPost] = useState(initialState);
  const [filter, setFilter] = useState({
    authorList: [],
    categoryList: [],
  });
  let { id } = useParams();
  id = id ?? 0;

  useEffect(() => {
    document.title = "Thêm/ cập nhật bài viết";

    getPostById(id).then((data) => {
      if (data) {
        setPost({
          ...data,
          selectedTags: data.tags.map((tag) => tag?.name).Join("\r\n"),
        });
      } else {
        setPost(initialState);
      }
    });
    getFilter().then((data) => {
      if (data) {
        setFilter({
          authorList: data.authorList,
          categoryList: data.categoryList,
        });
      } else {
        setFilter({
          authorList: [],
          categoryList: [],
        });
      }
    });
  }, []);
  const handleSubmit = (e) => {
    e.preventDefault();
    let form = new FormData(e.target);
    addOrUpdatePost(form).then((data) => {
      if (data) {
        alert("Đã lưu thành công");
      } else {
        alert("Đã xảy ra lỗi!");
      }
    });
  };
  if (id && !isInteger(id)) {
    return <Navigate to={`/400?redirecTo=/admin/posts`} />;
  } else {
    return (
      <>
        <Form
          method="post"
          encType="multipart/form-data"
          onSubmit={handleSubmit}
        >
          <Form.Control type="hidden" name="id" value={post.id} />
          <div className="row mb-3">
            <Form.Label className="col-sm=2 col-form=label">Tiêu đề</Form.Label>
            <div className="col-sm-10">
              <Form.Control
                type="text"
                name="title"
                title="Title"
                required
                value={post.title || ""}
                onChange={(e) =>
                  setPost({
                    ...post,
                    title: e.target.value,
                  })
                }
              />
            </div>
          </div>
          <div className="row mb-3">
            <Form.Label className="col-sm=2 col-form=label">Slug</Form.Label>
            <div className="col-sm-10">
              <Form.Control
                type="text"
                name="urlSlug"
                title="Url Slug"
                required
                value={post.urlSlug || ""}
                onChange={(e) =>
                  setPost({
                    ...post,
                    urlSlug: e.target.value,
                  })
                }
              />
            </div>
          </div>
          <div className="row mb-3">
            <Form.Label className="col-sm=2 col-form=label">
              Giới thiệu
            </Form.Label>
            <div className="col-sm-10">
              <Form.Control
                as="textarea"
                type="text"
                name="shortDescription"
                title="Short Description"
                required
                value={decode(post.shortDescription || "")}
                onChange={(e) =>
                  setPost({
                    ...post,
                    shortDescription: e.target.value,
                  })
                }
              />
            </div>
          </div>
          <div className="row mb-3">
            <Form.Label className="col-sm=2 col-form=label">
              Nội dung
            </Form.Label>
            <div className="col-sm-10">
              <Form.Control
                as="textarea"
                rows={10}
                type="text"
                name="description"
                title="Description"
                required
                value={decode(post.description || "")}
                onChange={(e) =>
                  setPost({
                    ...post,
                    description: e.target.value,
                  })
                }
              />
            </div>
          </div>
          <div className="row mb-3">
            <Form.Label className="col-sm=2 col-form=label">
              Metadata
            </Form.Label>
            <div className="col-sm-10">
              <Form.Control
                type="text"
                name="meta"
                title="meta"
                required
                value={decode(post.meta || "")}
                onChange={(e) =>
                  setPost({
                    ...post,
                    meta: e.target.value,
                  })
                }
              />
            </div>
          </div>
          <div className="row mb-3">
            <Form.Label className="col-sm=2 col-form=label">Tác giả</Form.Label>
            <div className="col-sm-10">
              <Form.Select
                name="authorId"
                title="Auhor Id"
                required
                value={post.author.id}
                onChange={(e) =>
                  setPost({
                    ...post,
                    author: e.target.value,
                  })
                }
              >
                <option value="">-- Chọn tác giả --</option>
                {filter.authorList.length > 0 &&
                  filter.authorList.map((item, index) => (
                    <option key={index} value={item.value}>
                      {item.text}
                    </option>
                  ))}
              </Form.Select>
            </div>
          </div>
          <div className="row mb-3">
            <Form.Label className="col-sm=2 col-form=label">Chủ đề</Form.Label>
            <div className="col-sm-10">
              <Form.Select
                name="categoryId"
                title="Category Id"
                required
                value={post.category.id}
                onChange={(e) =>
                  setPost({
                    ...post,
                    category: e.target.value,
                  })
                }
              >
                <option value="">-- Chọn chủ đề --</option>
                {filter.categoryList.length > 0 &&
                  filter.categoryList.map((item, index) => (
                    <option key={index} value={item.value}>
                      {item.text}
                    </option>
                  ))}
              </Form.Select>
            </div>
          </div>
          <div className="row mb-3">
            <Form.Label className="col-sm=2 col-form=label">
              Từ khóa (Mỗi từ 1 dòng)
            </Form.Label>
            <div className="col-sm-10">
              <Form.Control
                as="textarea"
                rows={5}
                type="text"
                name="selectedTags"
                title="selected Tags"
                required
                value={post.selectedTags}
                onChange={(e) =>
                  setPost({
                    ...post,
                    selectedTags: e.target.value,
                  })
                }
              />
            </div>
          </div>
          {!isEmptyOrSpaces(post.imageUrl) && (
            <div className="row mb-3">
              <Form.Label className="col-sm=2 col-form=label">
                Hình hiện tại
              </Form.Label>
              <div className="col-sm-10">
                <img src={post.imageUrl} alt={post.title} />
              </div>
            </div>
          )}
          <div className="row mb-3">
            <Form.Label className="col-sm=2 col-form=label">
              Chọn hình ảnh
            </Form.Label>
            <div className="col-sm-10">
              <Form.Control
                type="file"
                name="imageFile"
                accept="image/*"
                title="Image file"
                value={decode(post.meta || "")}
                onChange={(e) =>
                  setPost({
                    ...post,
                    ImageFile: e.target.files[0],
                  })
                }
              />
            </div>
          </div>
          <div className="row mb-3">
            <div className="col-sm-10 offset-sm-2">
              <div className="form-check">
                <input
                  type="checkbox"
                  className="form-check-input"
                  name="published"
                  checked={post.published}
                  title="published"
                  onChange={(e) =>
                    setPost({
                      ...post,
                      published: e.target.checked,
                    })
                  }
                />
                <Form.Label className="form-check-label">
                  Đã xuất bản
                </Form.Label>
              </div>
            </div>
          </div>
          <div className="text-center">
            <Button variant="primary" type="submit">
              Lưu các thay đổi
            </Button>
            <Link to="/admin/posts" className="btn btn-danger ms-2">
              Hủy và quay lại
            </Link>
          </div>
        </Form>
      </>
    );
  }
};

export default Edit;
