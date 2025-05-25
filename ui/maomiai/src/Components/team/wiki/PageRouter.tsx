import { RouteObject } from "react-router";
import Wiki from "./Wiki";
import WikiList from "./WikiList";
import WikiSetting from "./WikiSetting";
import WikiDocument from "./WikiDocument";
import WikiEmbedding from "./WikiEmbedding";
import WikiEmbeddingTest from "./WikiEmbeddingTest";

export const WikiPageRouter: RouteObject = {
  path: "wiki/:wikiId",
  Component: Wiki,
  children: [
    {
      path: "setting",
      Component: WikiSetting,
    },
    {
      path: "document",
      Component: WikiDocument,
    },
    {
      path: "embedding",
      Component: WikiEmbedding,
    },
    {
      path: "embeddingtest",
      Component: WikiEmbeddingTest,
    },
  ],
};

export const WikiListPageRouter: RouteObject = {
  path: "wikilist",
  Component: WikiList,
};
