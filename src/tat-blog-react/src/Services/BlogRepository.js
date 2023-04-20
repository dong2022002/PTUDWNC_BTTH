import axios from "axios";
import { get_api, post_api, put_api } from "./Method";

export function getPosts(
  keyword = "",
  pageSize = 10,
  pageNumber = 1,
  sortColumn = "",
  sortOrder = ""
) {
  return get_api(
    `https://localhost:7126/api/posts?Keyword=${keyword}&PublishedOnly=true&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`
  );
}
export function getAuthors(
  name = "",
  pageSize = 10,
  pageNumber = 1,
  sortColumn = "",
  sortOrder = ""
) {
  return get_api(
    `https://localhost:7126/api/authors?Name=${name}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`
  );
}

export async function getFeaturedPosts(linmit = 3) {
  try {
    const respone = await axios.get(
      `https://localhost:7126/api/posts/featured/${linmit}`
    );
    const data = respone.data;
    if (data.isSuccess) {
      return data.result;
    } else return null;
  } catch (error) {
    console.log("Error", error.message);
    return null;
  }
}
export async function getDetailPostsBySlug(slug) {
  try {
    const respone = await axios.get(
      `https://localhost:7126/api/posts/byslug/${slug}`
    );
    const data = respone.data;
    if (data.isSuccess) {
      return data.result;
    } else return null;
  } catch (error) {
    console.log("Error", error.message);
    return null;
  }
}
export async function getRandomPosts(limit) {
  try {
    const respone = await axios.get(
      `https://localhost:7126/api/posts/random/${limit}`
    );
    const data = respone.data;
    if (data.isSuccess) {
      return data.result;
    } else return null;
  } catch (error) {
    console.log("Error", error.message);
    return null;
  }
}

export async function getArchivesPosts(limit) {
  try {
    const respone = await axios.get(
      `https://localhost:7126/api/posts/archives/${limit}`
    );
    const data = respone.data;
    if (data.isSuccess) {
      return data.result;
    } else return null;
  } catch (error) {
    console.log("Error", error.message);
    return null;
  }
}

export function getFilter() {
  return get_api("https://localhost:7126/api/posts/get-filter");
}

export function getPostFilter(
  keyword = "",
  authorId = "",
  categoryId = "",
  year = "",
  month = "",
  pageSize = 10,
  pageNumber = 1,
  sortColumn = "",
  sortOrder = ""
) {
  let url = new URL("https://localhost:7126/api/posts?PublishedOnly=false");
  keyword !== "" && url.searchParams.append("Keyword", keyword);
  authorId !== "" && url.searchParams.append("AuthorId", authorId);
  categoryId !== "" && url.searchParams.append("CategoryId", categoryId);
  year !== "" && url.searchParams.append("Year", year);
  month !== "" && url.searchParams.append("Month", month);
  sortColumn !== "" && url.searchParams.append("SortColumn", sortColumn);
  sortOrder !== "" && url.searchParams.append("SortOrder", sortOrder);
  url.searchParams.append("PageSize", pageSize);
  url.searchParams.append("PageNumber", pageNumber);
  return get_api(url.href);
}

export function getPostById(id) {
  if (id > 0) {
    return get_api(`https://localhost:7126/api/posts/${id}`);
  }
  return null;
}
export async function addOrUpdatePost(formData) {
  return await post_api("https://localhost:7126/api/posts", formData);
}

export async function changePublished(id) {
  return await put_api(
    `https://localhost:7126/api/posts/${id}/changePublished`
  );
}
