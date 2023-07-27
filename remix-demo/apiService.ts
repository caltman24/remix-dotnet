import { fetch as remixFetch } from "@remix-run/node";
import { TodoModel } from "todoService";

export default class TodoApiService {
  private static readonly baseUrl = "http://localhost:5000";

  static async getAll(token: string): Promise<TodoModel[]> {
    return (await this.fetchApi<TodoModel[]>("/todos", token)) ?? [];
  }

  static async getById(
    id: number,
    token: string
  ): Promise<TodoModel | undefined> {
    return await this.fetchApi<TodoModel>("/todos/" + id, token);
  }

  static async add(todo: TodoModel, token: string): Promise<void> {
    await this.fetchApi("/todos", token, JSON.stringify(todo), "POST");
  }

  static async delete(id: number, token: string): Promise<void> {
    await this.fetchApi(`/todos/${id}`, token, "", "DELETE");
  }

  private static async fetchApi<T>(
    url: string,
    token?: string,
    body?: string,
    method?: string
  ): Promise<T | undefined> {
    const fetchUrl = url.startsWith("/")
      ? this.baseUrl + url
      : `${this.baseUrl}/${url}`;

    const res = await remixFetch(fetchUrl, {
      headers: {
        "Content-Type": "Application/Json",
        Authorization: `bearer ${token}`,
      },
      body,
      method,
    });

    if (typeof method !== "undefined" && method.toLowerCase() !== "get")
      return undefined;

    if (res.status === 404) return undefined;

    if (res.status === 401) {
      console.log("Unauthorized Request");
      return undefined;
    }

    if (!res.ok) {
      throw new Error("There was a problem with the TodoMe API");
    }

    const data: T = await res.json();
    return data;
  }
}
