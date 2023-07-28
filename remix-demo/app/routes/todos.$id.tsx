
import { getAuth } from "@clerk/remix/ssr.server";
import { LoaderArgs, json } from "@remix-run/node";
import { Form, Link, isRouteErrorResponse, useLoaderData, useNavigation, useRouteError } from "@remix-run/react";
import TodoApiService from "apiService";
import { DefaultJwtTemplate } from "~/root";


export async function loader(args: LoaderArgs) {
    const { getToken } = await getAuth(args);

    const token = await getToken(DefaultJwtTemplate);

    const todo = await TodoApiService.getById(parseInt(args.params.id!), token!);


    if (!todo) throw new Response("Todo Not Found", { status: 404 })

    return json(todo);
}

export function ErrorBoundary() {
    const error = useRouteError();

    if (isRouteErrorResponse(error)) {
        return (
            <div>
                <Link to={"/todos"}>{"<-"} Go Back</Link>
                <p>{error.data}</p>
            </div>
        )
    }

    return <h1>There was an unexpected error</h1>

}


export default function TodoByIdApiRoute() {
    const todo = useLoaderData<typeof loader>();
    const navigation = useNavigation();
    const isDeleting = navigation.state === "submitting" && navigation.formMethod === "POST"

    return <div>
        <Link to={"/todos"} className="py-1 px-4 rounded-lg bg-cyan-300 transition-[background] hover:transition-[background] hover:bg-cyan-200 active:transition-[background] active:bg-cyan-400">Go Back</Link>

        <div className="w-fit min-w-[200px] p-4 flex flex-col gap-2 bg-slate-200 rounded-lg mt-5">
            <h1 className="text-lg font-medium">{todo.title}</h1>
            <p>{todo.id}</p>

            {
                isDeleting ? (
                    <p>Deleting...</p>
                )
                    : (<Form method="post" action="/todos" >
                        <input type="text" value={todo.id} name="id" hidden readOnly />
                        <button type="submit" className="py-1 px-4 bg-rose-300 rounded-lg transition-[background] hover:transition-[background] hover:bg-rose-200 active:transition-[background] active:bg-rose-400">Delete</button>
                    </Form>)
            }
        </div>
    </div >
}