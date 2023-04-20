import { faArrowLeft, faArrowRight } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import Button from "react-bootstrap/Button";
import { Link } from "react-router-dom";

const PagerAdmin = ({ metadata, controller = "", actionName = "" }) => {
  let pageCount = metadata.pageCount,
    hasNextPage = metadata.hasNextPage,
    hasPreviousPage = metadata.hasPreviousPage,
    pageNumber = metadata.pageNumber,
    pageSize = metadata.pageSize,
    slug = "";
  if (pageCount > 1) {
    return (
      <div className="text-center my-4">
        {hasPreviousPage ? (
          <Link
            to={`${controller}/${actionName}?slug=${slug}&p=${
              pageNumber - 1
            }&ps=${pageSize}`}
            className="btn btn-info"
          >
            <FontAwesomeIcon icon={faArrowLeft} />
            &nbsp;Trang trước
          </Link>
        ) : (
          <Button variant="outline-secondary" disabled>
            <FontAwesomeIcon icon={faArrowLeft} />
            &nbsp;Trang trước
          </Button>
        )}
        {hasNextPage ? (
          <Link
            to={`${controller}/${actionName}?slug=${slug}&p=${
              pageNumber + 1
            }&ps=${pageSize}`}
            className="btn btn-info ms-1"
          >
            <FontAwesomeIcon icon={faArrowRight} />
            Trang sau&nbsp;
          </Link>
        ) : (
          <Button variant="outline-secondary" className="ms-1" disabled>
            <FontAwesomeIcon icon={faArrowRight} />
            Trang sau&nbsp;
          </Button>
        )}
      </div>
    );
  }
  return <Link></Link>;
};

export default PagerAdmin;
