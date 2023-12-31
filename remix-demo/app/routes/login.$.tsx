import { SignIn } from "@clerk/remix";
import { getAuth } from "@clerk/remix/ssr.server";
import { LoaderArgs, redirect } from "@remix-run/node";
import { DefaultJwtTemplate } from "~/root";

export async function loader(args: LoaderArgs) {
    const { userId, getToken } = await getAuth(args);

    if (!userId) return null

    const token = await getToken(DefaultJwtTemplate);

    // send request to add user to database 
    await fetch("http://localhost:5000/users", {
        method: "POST",
        headers: {
            "Authorization": `bearer ${token}`
        }
    })

    return redirect("/todos")

}

export default function SignInPage() {
    return (
        <div className="flex justify-center mt-10">
            <SignIn routing={"path"} path={"/login"} redirectUrl={"/login"} />
        </div>
    );
}