

## 开发说明

### 配置

项目支持环境变量和文件注入配置，建议统一 configs 目录统一管理配置文件。

创建环境变量，`MAI_CONFIG`，设置变量值为配置文件路径，配置文件支持 `.json`、`.yaml`、`.conf` 等类型。

如：

```
MAI_CONFIG = E:/configs/maiconfigs.json
```

![image-20250309210715585](images/image-20250309210715585.png)



使用 docker 启动时，可以通过 `docker -v /data/config:/app/configs -e MAI_CONFIG=/app/configs/system.yaml` 的形式向服务提供配置文件。