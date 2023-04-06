import axios from "axios";

export async function getPosts(keyword = '' , pageSize = 10, pageNumber =1, sortColumn = '', sortOrder ='') {
    try {
        const respone = await axios.get(`https://localhost:7126/api/posts?Keyword=${keyword}&PublishedOnly=true&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
        const data  = respone.data;
        if (data.isSuccess) {
            return data.result;
        }else
        return null;
    } catch (error) {
        console.log('Error',error.message);
        return null;
    }
}

export async function getFeaturedPosts(linmit =3) {
  try {
      const respone = await axios.get(`https://localhost:7126/api/posts/featured/${linmit}`);
      const data  = respone.data;
      if (data.isSuccess) {
          return data.result;
      }else
      return null;
  } catch (error) {
      console.log('Error',error.message);
      return null;
  }
}
