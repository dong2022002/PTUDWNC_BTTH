import React, { useEffect,useState } from 'react'
import PostItem from '../components/PostItem'
import { getPosts } from '../Services/BlogRepository';
import {useQuery} from '../Utils/Utils';
import Pager from '../components/Pager';

const Index = () => {
  const [postList,setPostList] = useState([]);
  const [metadata,setMetadata] = useState([]);

  let query = useQuery(),
    k = query.get('k') ?? '',
    p = query.get('p') ?? 1,
    ps = query.get('ps')?? 10;

  useEffect(() => {
    document.title = 'Trang chủ';

    getPosts(k,ps,p).then(data =>{
      if (data) {
        setPostList(data.items);
        setMetadata(data.metadata)
      }else
      setPostList([]);
    })
  }, [k,p,ps]);

  useEffect(()=>{
    window.scrollTo(0,0);
  },[postList]);

  if (postList.length>0) {
      return (
      <div className="p-4">
        {postList.map((item,index) =>{
          return(
            <PostItem postItem={item} key={index}/>
          )
        })}
        <Pager postQuery={{'keyword':k}} metadata={metadata}/>
      </div>
    )
  }else return(
    <></>
  )
}

export default Index
