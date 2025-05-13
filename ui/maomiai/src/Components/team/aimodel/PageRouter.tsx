/*
 * @Author: whuanle 1586052146@qq.com
 * @Date: 2025-05-05 14:52:09
 * @LastEditors: whuanle 1586052146@qq.com
 * @LastEditTime: 2025-05-05 14:55:23
 * @FilePath: \maomiai\src\Components\aimodel\PageRouter.tsx
 * @Description: 这是默认设置,请设置`customMade`, 打开koroFileHeader查看配置 进行设置: https://github.com/OBKoro1/koro1FileHeader/wiki/%E9%85%8D%E7%BD%AE
 */
import {
    RouteObject
} from "react-router";
import AiModel from "./AiModel";

export const AiModelPageRouter: RouteObject = {
    path: 'aimodel',
    Component: AiModel
}