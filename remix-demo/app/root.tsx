import type { LinksFunction, LoaderFunction } from "@remix-run/node";
import {
  Link,
  Links,
  LiveReload,
  Meta,
  Outlet,
  Scripts,
  ScrollRestoration,
} from "@remix-run/react";
import stylesheet from "~/tailwind.css"

import { rootAuthLoader } from "@clerk/remix/ssr.server"
import { ClerkApp } from "@clerk/remix/dist/client/ClerkApp";
import { SignedIn, SignedOut, V2_ClerkErrorBoundary, UserButton } from "@clerk/remix";

export const DefaultJwtTemplate = {
  template: "TodoMe"
} as const

export const links: LinksFunction = () => [
  { rel: "stylesheet", href: stylesheet }
];

export const loader: LoaderFunction = (args) => rootAuthLoader(args)

export const ErrorBoundary = V2_ClerkErrorBoundary();

export function App() {
  return (
    <html lang="en">
      <head>
        <meta charSet="utf-8" />
        <meta name="viewport" content="width=device-width,initial-scale=1" />
        <Meta />
        <Links />
      </head>
      <body className="bg-slate-100">
        <header className="mb-5">
          <nav className="max-w-[1400px] m-auto flex gap-8 items-center justify-between px-2 py-3">
            <Link prefetch="render" to="/">
              <h1 className="text-2xl font-bold">Todo<span className="text-blue-800">Me</span></h1>
            </Link>

            <ul className="flex gap-5 font-medium items-center">
              <SignedIn>
                <li>
                  <Link prefetch="render" to="/todos" className="hover:text-blue-300">Todos</Link>
                </li>
                <li>
                  <UserButton />
                </li>
              </SignedIn>

              <SignedOut>
                <li>
                  <Link prefetch="render" to="/login" className="hover:text-blue-300">Login</Link>
                </li>
              </SignedOut>
            </ul>
          </nav>
        </header>
        <main className="max-w-[1400px] mx-auto px-2">
          <Outlet />
        </main>
        <ScrollRestoration />
        <Scripts />
        <LiveReload />
      </body>
    </html>
  );
}

export default ClerkApp(App);
