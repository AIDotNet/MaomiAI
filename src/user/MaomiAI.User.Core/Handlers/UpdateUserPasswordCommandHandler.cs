using Maomi.AI.Exceptions;
using MaomiAI.Database;
using MaomiAI.Infra.Helpers;
using MaomiAI.Infra.Models;
using MaomiAI.Infra.Services;
using MaomiAI.User.Shared.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace MaomiAI.User.Core.Handlers;

public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, EmptyCommandResponse>
{
    private readonly UserContext _userContext;
    private readonly DatabaseContext _dbContext;
    private readonly IRsaProvider _rsaProvider;

    public UpdateUserPasswordCommandHandler(UserContext userContext, DatabaseContext dbContext, IRsaProvider rsaProvider)
    {
        _userContext = userContext;
        _dbContext = dbContext;
        _rsaProvider = rsaProvider;
    }

    public async Task<EmptyCommandResponse> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.Where(x => x.Id == _userContext.UserId).FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new BusinessException("用户不存在") { StatusCode = 400 };
        }

        if (!user.IsEnable)
        {
            throw new BusinessException("用户已被禁用") { StatusCode = 400 };
        }

        // 使用 RSA 解密还原密码
        string restorePassword = default!;
        try
        {
            restorePassword = _rsaProvider.Decrypt(request.Password);
            Regex regex = new Regex(@"^(?![0-9]+$)(?![a-zA-Z]+$)(?![0-9a-zA-Z]+$)(?![0-9\\W]+$)(?![a-zA-Z\\W]+$)[0-9A-Za-z\\W]{8,30}$");
            Match match = regex.Match(restorePassword);
            if (!match.Success)
            {
                throw new BusinessException("密码 8-30 长度，并包含数字+字母+特殊字符.") { StatusCode = 400 };
            }
        }
        catch
        {
            throw new BusinessException("密码验证失败") { StatusCode = 400 };
        }

        // 使用 PBKDF2 算法生成哈希值
        var (hashedPassword, saltBase64) = PBKDF2Helper.ToHash(restorePassword);
        user.Password = hashedPassword;
        user.PasswordHalt = saltBase64;

        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return EmptyCommandResponse.Default;
    }
}
