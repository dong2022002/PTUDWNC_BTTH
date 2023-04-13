import Card from "react-bootstrap/Card";
import { Link } from "react-router-dom";
import { isEmptyOrSpaces } from "../Utils/Utils";
import TagList from "./TagList";
const PostItem = ({ postItem }) => {
  let imageUrl = isEmptyOrSpaces(postItem.imageUrl)
    ? process.env.PUBLIC_URL + "/images/image_1.jpg"
    : `${postItem.imageUrl}`;
  let date = new Date(postItem.postedDate);
  return (
    <article className="blog-entry mb-4">
      <Card>
        <div className="row g-0">
          <div className="col-md-4">
            <Card.Img
              variant="top"
              style={{ width: "300px", height: "250px" }}
              src={imageUrl}
              alt={postItem.title}
            />
          </div>
          <div className="col-md-8">
            <Card.Body>
              <Card.Title>{postItem.title}</Card.Title>
              <Card.Text>
                <small className="text-muted">Tác giả</small>
                <Link
                  to={`blog/author/${postItem.urlSlug}`}
                  className="text-primary m-1 text-decoration-none"
                >
                  {postItem.author.fullName}
                </Link>
                <small className="text-muted">Chủ đề</small>
                <Link
                  to={`blog/author/${postItem.urlSlug}`}
                  className="text-primary m-1 text-decoration-none"
                >
                  {postItem.category.name}
                </Link>
              </Card.Text>
              <Card.Text>{postItem.shortDescription}</Card.Text>
              <div className="tag-list">
                <TagList tagList={postItem.tags} />
              </div>
              <div className="text-end">
                <Link
                  to={`/blog/post?year=${date.getFullYear()}&month=${
                    date.getMonth() + 1
                  }&day=${date.getDate()}&slug=${postItem.urlSlug}`}
                  className="btn btn-primary"
                  title={postItem.title}
                >
                  Xem chi tiết
                </Link>
              </div>
            </Card.Body>
          </div>
        </div>
      </Card>
    </article>
  );
};

export default PostItem;
