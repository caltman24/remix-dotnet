import { getAuth } from "@clerk/remix/ssr.server";
import { ActionArgs, LoaderArgs, redirect } from "@remix-run/node";
import { Link, Outlet, isRouteErrorResponse, useLoaderData, useNavigation, useRouteError } from "@remix-run/react";
import TodoApiService from "apiService";
import { TodoModel } from '../../todoService';


const pageTitle = <h1 className="font-bold text-3xl mb-7">Todos from .NET API</h1>

export async function loader(args: LoaderArgs) {
    const { userId, getToken } = await getAuth(args);

    if (!userId) return redirect("/login")

    const token = await getToken({ template: "TodoMe" });

    return await TodoApiService.getAll(token!);
}

export async function action(args: ActionArgs) {
    const formData = await args.request.formData();
    const id = formData.get("id");

    const { getToken } = await getAuth(args);

    const token = await getToken({ template: "TodoMe" });

    await TodoApiService.delete(parseInt(id?.toString()!), token!)

    return redirect("/todos")
}

export function ErrorBoundary() {
    const error = useRouteError();

    if (isRouteErrorResponse(error)) {
        return (
            <div>
                <h1>{error.status} {error.statusText}</h1>
                <p>{error.data}</p>
            </div>
        )
    }

    if (error instanceof Error) {
        return (<div>
            {pageTitle}
            <h2 className="text-2xl font-semibold">Oh Snap!</h2>
            <p>There was a problem loading your todos</p>
        </div>)
    }

    return <p>There was an error that we did not anticipate</p>
}

export default function TodosApiRootRoute() {
    const todos = useLoaderData<TodoModel[]>();


    return (
        <div>
            {pageTitle}

            <Outlet />

            <ul className="h-52 overflow-y-auto mt-5 flex flex-col gap-5">
                {todos.map(t => (
                    <li key={t.id}>
                        <Link prefetch="viewport" to={`/todos/${t.id}`}>
                            <div className={`${t.isComplete ? "bg-rose-200" : "bg-slate-100 hover:bg-cyan-100 active:bg-slate-200 transition-[background] hover:transition-[background] active:transition-[background]"} py-2 px-6 rounded-lg`}>
                                <p className="font-medium">{t.title}</p>
                            </div>
                        </Link>
                    </li>))}
            </ul>
        </div>)
}