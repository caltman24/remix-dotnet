import { getAuth } from "@clerk/remix/ssr.server";
import { ActionArgs } from "@remix-run/node";
import { Form, useNavigation } from "@remix-run/react";
import TodoApiService from "apiService";
import { useEffect, useRef } from "react";
import { TodoModel } from "todoService";

type TodoFormModel = Pick<TodoModel, "title">

function serializeForm<T extends object>(formData: FormData): Partial<T> {
    const output: Partial<T> = {}

    for (const [key, value] of formData.entries()) {
        if (!(key in output)) {
            output[key as keyof T] = value.toString() as T[keyof T]
        }
    }

    return output
}

export async function action(args: ActionArgs) {
    const form = await args.request.formData();
    const formData = serializeForm<TodoFormModel>(form);

    const newTodo: TodoModel = {
        id: 0,
        title: formData.title!,
        isComplete: false
    }

    const { getToken } = await getAuth(args)

    const token = await getToken({ template: "TodoMe" });

    await TodoApiService.add(newTodo, token!);

    return null
}

export default function TodosApiIndexRoute() {
    const navigation = useNavigation();
    const isAdding = navigation.state === "submitting"
        && navigation.formMethod === "post"

    const formRef = useRef<HTMLFormElement>(null);

    // FIXME: NOT WOKRING!
    useEffect(() => {
        if (isAdding) return

        formRef.current?.reset();
    }, [isAdding])

    return (
        <ul>
            <div>
                <h1 className="text-lg font-medium mb-2">Add new todo </h1>
                <Form method="post" ref={formRef}>
                    <div className="flex flex-col gap-2">
                        <label>Title: </label>
                        <div className="flex gap-2">
                            <input type="text" name="title" className="bg-slate-200 rounded-lg px-4 py-1" minLength={1} maxLength={50} required />
                            <button type="submit" className="button px-4 py-1 bg-cyan-300 transition-[background] hover:transition-[background] hover:bg-cyan-200 active:bg-cyan-400 active:transition-[background] rounded-lg">
                                Add
                            </button>
                        </div>
                    </div>
                </Form>
            </div>
        </ul>
    )
}