import { RouteObject } from "react-router";
import Note from "./note/Note";
import UserChat from "./chat/Chat";

export const UserApplicationPageRouter: RouteObject[] = [
  {
    path: ":/userapp/note",
    Component: Note,
  },
  {
    path: ":/userapp/chat",
    Component: UserChat,
  },
];
