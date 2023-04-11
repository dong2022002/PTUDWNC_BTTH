
import { get_api } from "./Method";

export async function getCategories() {
    return get_api(`https://localhost:7126/api/categories?PageSize=10&PageNumber=1`);
}
