/*
 * @Author: whuanle 1586052146@qq.com
 * @Date: 2025-05-04 09:20:05
 * @LastEditors: whuanle 1586052146@qq.com
 * @LastEditTime: 2025-05-04 20:59:36
 * @FilePath: \maomiai\src\helper\RsaHalper.tsx
 * @Description: 这是默认设置,请设置`customMade`, 打开koroFileHeader查看配置 进行设置: https://github.com/OBKoro1/koro1FileHeader/wiki/%E9%85%8D%E7%BD%AE
 */
import JSEncrypt from 'jsencrypt';

export class RsaHelper {
    /**
     * RSA加密
     * @param data 要加密的数据
     * @returns 加密后的数据
     */
    public static encrypt(publicKey: string, data: string): string {

        let encryptor = new JSEncrypt();
        encryptor.setPublicKey(publicKey);
        let result =  encryptor.encrypt(data);
        if(!result){
            throw new Error('加密错误');
        }

        // 后端统一 2048
        return btoa(atob(result).padStart(2048, "\0"));
    }
}
