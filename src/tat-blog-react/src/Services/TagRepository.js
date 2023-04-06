import axios from "axios";

export async function getTags(ps=10,p =1) {
    try {
        const respone = await axios.get(`https://localhost:7126/api/tags?PageSize=${ps}&PageNumber=${p}`);
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
