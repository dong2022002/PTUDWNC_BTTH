import React, { useEffect, useState } from 'react'
import { getFeaturedPosts } from '../Services/BlogRepository';
import { Link } from 'react-router-dom';
import { ListGroup } from 'react-bootstrap';

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
    <div className="mb-4">
        <h3 className='text-success mb-2'>
            Bài viết nổi bật
        </h3>
        {postList.length > 0 &&
            <ListGroup>
                {postList.map((item,index)=>{
                    return(
                        <ListGroup.Item key={index}>
                            <Link to={`/blog/post?slug=${item.urlSlug}`}
                                title={item.title}
                                key={index}>
                                {item.title}
                                <span>&nbsp;({item.viewCount})</span>
                            </Link>
                        </ListGroup.Item>
                    )
                })}
            </ListGroup>

        }
    </div>
)
}else return(
<></>
)
}
export default FeaturedPost
