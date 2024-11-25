const path = require("path");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const webpack = require("webpack");

module.exports = {
    mode: process.env.NODE_ENV === 'development' ? 'development' : 'production',
    entry: {
        main: {
            import: path.join(__dirname, "ClientApp", "main.ts"),
        },
    },
    output: {
        path: path.resolve(__dirname, "wwwroot/app"),
        filename: "[name].bundle.js",
        //filename: "[name].[chunkhash].js",
        publicPath: "/app/",
        //libraryTarget: 'var',
        library: 'web',
        assetModuleFilename: 'images/[hash][ext][query]'
    },
    resolve: {
        extensions: [".js", ".ts"]
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: "ts-loader"
            },
            {
                test: /\.s[ac]ss$/i,
                use: [MiniCssExtractPlugin.loader, "css-loader", "sass-loader"],
            },
            // For webpack v5
            {
                test: /\.(png|jpe?g|gif)$/i,
                // More information here https://webpack.js.org/guides/asset-modules/
                type: "asset"
            },
        ]
    },
    optimization: {
        splitChunks: {
            cacheGroups: {
                // defaultVendors: {
                //     filename: '[name].bundle.js',
                // },
                vendor: {
                    test: /[\\/]node_modules[\\/](bootstrap|kendo-ui-core)[\\/]/,
                    name: 'vendor',
                    chunks: 'all',
                },
            },
        }
    },
    plugins: [
        new webpack.ProvidePlugin({
            // $: 'jquery',
            // jQuery: 'jquery',

            kendo: 'kendo-ui-core',
            'window.kendo': 'kendo-ui-core'
        }),
        //new ArcGISPlugin(),
        new CleanWebpackPlugin(),
        //new HtmlWebpackPlugin({
        //    template: "./src/index.html"
        //}),
        new MiniCssExtractPlugin({
            filename: "[name].css"
            //filename: "css/[name].[chunkhash].css"
        })
    ]
};