import React, { useEffect, useState } from 'react'
import { getFeaturedPosts } from '../Services/BlogRepository';
import { isEmptyOrSpaces } from '../Utils/Utils';
import { Link } from 'react-router-dom';

const FeaturedPost = () => {
  const [postList,setPostList] = useState([]);
  useEffect(() => {
    document.title = 'Trang chủ';

    getFeaturedPosts(3).then(data =>{
      if (data) {
        setPostList(data);
      }else
      setPostList([]);
    })
  }, []);

if (postList.length>0) {
  return (
  <div className="p-4">
    {
      postList.map((postItem,index) => {
        let date = new Date(postItem.postedDate);
        let imageUrl = isEmptyOrSpaces(postItem.imageUrl)
        ? process.env.PUBLIC_URL + '/images/image_1.jpg'
        : `${postItem.imageUrl}`;
        return (
          <article className="mb-1" key={index}>
             <Link
             to={`/blog/post?year=${date.getFullYear()}&month=${date.getMonth()}&day=${date.getDay()}&slug=${postItem.urlSlug}`}
             className="float-start text-decoration-none">
              <div className="d-flex align-items-center">
              <img className="media-object rounded"
                  alt="64x64"
                  style={{width:"60px",height:"60px"}}
                  src={imageUrl}
                  data-holder-rendered="true" />
                <h6 className="text-decoration-none w-auto m-4 ">
                  { postItem.author.fullName}
                </h6>
              </div>
              <h6 className="position-relative w-auto">
                {postItem.title}
              </h6>
              <div className="d-block">
                {postItem.shortDescription}
              </div>
             </Link>


          </article>)},)
    }
  </div>
)
}else return(
<></>
)
}
export default FeaturedPost
