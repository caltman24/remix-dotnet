import type { V2_MetaFunction } from "@remix-run/node";
import { Link, Outlet } from "@remix-run/react";

export const meta: V2_MetaFunction = () => {
  return [
    { title: "TodoMe" },
    { name: "description", content: "Welcome to TodoMe!" },
  ];
};

export default function Index() {
  return (
    <div>
      <h1>Welcome to TodoMe</h1>
    </div>
  );
}
