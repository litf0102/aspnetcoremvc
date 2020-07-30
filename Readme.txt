■ソリューション構成
本プロジェクトは、以下のように分割したい。
①ASP.NET Core MVCプロジェクト
②Entityプロジェクト（クラスライブラリ）
③Repositoryプロジェクト（クラスライブラリ）
④Serviceプロジェクト（クラスライブラリ）
⑤共通処理プロジェクト（クラスライブラリ）
⑥ASP.NET Core WebAPIプロジェクト

※注意事項
①AutoMapperを共通プロジェクトに移動
②AutoFacでDIを実現する場合、RedisCacheを利用する。
RedisCacheをIoCコンテナとして使用する。
③トークン検証は、JWTを利用する。
・MVCの場合、SessionとCookieを使用する。
・WebAPIの場合、Tokenを使用する。
