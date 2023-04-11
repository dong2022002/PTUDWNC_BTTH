import axios from "axios";
import { get_api } from "./Method";

export function getPosts(keyword = '', pageSize = 10, pageNumber = 1, sortColumn = '', sortOrder = '') {
    return get_api(`https://localhost:7126/api/posts?Keyword=${keyword}&PublishedOnly=true&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
}
export function getAuthors(name = '', pageSize = 10, pageNumber = 1, sortColumn = '', sortOrder = '') {
  return get_api(`https://localhost:7126/api/authors?Name=${name}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
}

export async function getFeaturedPosts(linmit = 3) {
    try {
        const respone = await axios.get(`https://localhost:7126/api/posts/featured/${linmit}`);
        const data = respone.data;
        if (data.isSuccess) {
            return data.result;
        } else
            return null;
    } catch (error) {
        console.log('Error', error.message);
        return null;
    }
}
export async function getDetailPostsBySlug(slug) {
    try {
        const respone = await axios.get(`https://localhost:7126/api/posts/byslug/${slug}`);
        const data = respone.data;
        if (data.isSuccess) {
            return data.result;
        } else
            return null;
    } catch (error) {
        console.log('Error', error.message);
        return null;
    }
}
export async function getRandomPosts(limit) {
    try {
        const respone = await axios.get(`https://localhost:7126/api/posts/random/${limit}`);
        const data = respone.data;
        if (data.isSuccess) {
            return data.result;
        } else
            return null;
    } catch (error) {
        console.log('Error', error.message);
        return null;
    }
}

export async function getArchivesPosts(limit) {
    try {
        const respone = await axios.get(`https://localhost:7126/api/posts/archives/${limit}`);
        const data = respone.data;
        if (data.isSuccess) {
            return data.result;
        } else
            return null;
    } catch (error) {
        console.log('Error', error.message);
        return null;
    }
}
