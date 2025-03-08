using System.Security.Cryptography;

namespace MaomiAI.Infra.Helpers;

/// <summary>
/// Pbkdf2.
/// </summary>
public static class Pbkdf2Helper
{
    // size in bytes，salt 的大小.
    private const int SALTSIZE = 128;

    // number of pbkdf2 iterations，表示循环次数.
    private const int ITERATIONS = 1000;

    /// <summary>
    /// 随机生成 salt，相当于生成密钥.
    /// </summary>
    /// <returns>Salt.</returns>
    public static byte[] GenerateSalt()
    {
        // 生成盐
        using RandomNumberGenerator provider = RandomNumberGenerator.Create();
        byte[] salt = new byte[SALTSIZE];
        provider.GetBytes(salt);
        return salt;
    }

    /// <summary>
    /// 随机生成 salt，相当于生成密钥，同时返回此密钥的字符串.
    /// </summary>
    /// <param name="saltBase64">Salt 的 base64 .</param>
    /// <returns>Salt.</returns>
    public static byte[] GenerateSalt(out string saltBase64)
    {
        var bytes = GenerateSalt();
        saltBase64 = SaltToBase64String(bytes);
        return bytes;
    }

    /// <summary>
    /// 将 salt 转换为 Base64 字符串.
    /// </summary>
    /// <param name="bytes">Salt.</param>
    /// <returns>Salt 字符串.</returns>
    public static string SaltToBase64String(byte[] bytes)
    {
        var text = Convert.ToBase64String(bytes);
        return text;
    }

    /// <summary>
    /// 将 base64 字符串转换为 byte[].
    /// </summary>
    /// <param name="saltBase64">base64 字符串.</param>
    /// <returns>Salt.</returns>
    public static byte[] Base64StringToSalt(string saltBase64)
    {
        var bytes = Convert.FromBase64String(saltBase64);
        return bytes;
    }

    /// <summary>
    /// 使用 salt 给一个字符串加密.
    /// </summary>
    /// <param name="text">待加密的字符串.</param>
    /// <param name="salt">Salt.</param>
    /// <param name="base64">加密后的 base64 字符串.</param>
    /// <returns>加密后的字符串字节.</returns>
    public static byte[] Encrypt(string text, byte[] salt, out string base64)
    {
        var bytes = Rfc2898DeriveBytes.Pbkdf2(
            password: text,
            salt: salt,
            iterations: ITERATIONS,
            hashAlgorithm: HashAlgorithmName.SHA1,
            outputLength: 32);

        base64 = Convert.ToBase64String(bytes);
        return bytes;
    }

    /// <summary>
    /// 使用 salt 给一个字符串加密.
    /// </summary>
    /// <param name="text">待加密的字符串.</param>
    /// <param name="saltBase64">Salt.</param>
    /// <param name="base64">加密后的 base64 字符串.</param>
    /// <returns>加密后的字符串字节.</returns>
    public static byte[] Encrypt(string text, string saltBase64, out string base64)
    {
        var bytes = Rfc2898DeriveBytes.Pbkdf2(
            password: text,
            salt: Base64StringToSalt(saltBase64),
            iterations: ITERATIONS,
            hashAlgorithm: HashAlgorithmName.SHA1,
            outputLength: 32);

        base64 = Convert.ToBase64String(bytes);
        return bytes;
    }

    /// <summary>
    /// 将未加密的字符串与加密后的字符串对比，判断是否相同.
    /// </summary>
    /// <param name="sourceText">未加密的字符串.</param>
    /// <param name="salt">Salt.</param>
    /// <param name="targetBase64">已加密的字符串.</param>
    /// <returns>是否相等.</returns>
    public static bool Check(string sourceText, string salt, string targetBase64)
    {
        return Check(sourceText, Base64StringToSalt(salt), targetBase64);
    }

    /// <summary>
    /// 将未加密的字符串与加密后的字符串对比，判断是否相同.
    /// </summary>
    /// <param name="sourceText">未加密的字符串.</param>
    /// <param name="salt">Salt.</param>
    /// <param name="targetBase64">已加密的字符串.</param>
    /// <returns>是否相等.</returns>
    public static bool Check(string sourceText, byte[] salt, string targetBase64)
    {
        _ = Encrypt(sourceText, salt, out var base64);
        return base64 == targetBase64;
    }
}
