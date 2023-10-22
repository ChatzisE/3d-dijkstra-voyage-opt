import PathRequest from "../models/pathRequest";
const common = {
    getGreatCirclePath: async function <T>(payload: PathRequest): Promise<T> {
        const myHeaders = new Headers();
        myHeaders.append("Content-Type", "application/json");
        const requestOptions = {
            method: 'POST',
            headers: myHeaders,
            body: JSON.stringify(payload)
        };
        const url = `http://localhost:5228/api/great-circle/${payload.step}`;
        const response = await fetch(url, requestOptions);
        if (!response.ok) {
            throw new Error(`Failed to fetch ${url}: ${response.status} ${response.statusText}`);
        }
        return response.json() as Promise<T>;
    }
}
export default common;