import axios from "axios";

export async function getCategories() {
    try {
        const respone = await axios.get(`https://localhost:7126/api/categories?PageSize=10&PageNumber=1`);
        const data  = respone.data;
        if (data.isSuccess) {
            return data.result.items;
        }else
        return null;
    } catch (error) {
        console.log('Error',error.message);
        return null;
    }
}