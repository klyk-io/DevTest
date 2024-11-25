const webpack = require('webpack');
const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const ArcGISPlugin = require("@arcgis/webpack-plugin");

module.exports = {
    mode: process.env.NODE_ENV === 'development' ? 'development' : 'production',
    entry: {
        main: {
            import: path.join(__dirname, "ClientApp", "main.ts"),            
            library: {
                name: "klyk",
                type: "var"
            },
        },
        //vendor: ["bootstrap", "kendo-ui-core", "tinymce"],
    },
    devtool: (process.env.NODE_ENV === 'development') ? 'inline-source-map' : false,
    output: {
        path: path.resolve(__dirname, "wwwroot/app"),
        filename: "[name].bundle.js",
        chunkFilename: '[name].[contenthash].js',
        //filename: "[name].[chunkhash].js",
        publicPath: "/_content/Klyk/app/",
        // libraryTarget: 'var',
        library: 'klyk',
        assetModuleFilename: 'images/[hash][ext][query]',
        clean: true
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
                use: [MiniCssExtractPlugin.loader, "css-loader", {
                    loader: "sass-loader",
                    options: {
                        // Prefer `dart-sass`
                        implementation: require.resolve("sass"),
                        sassOptions: {
                            indentWidth: 4,
                            includePaths: ["./ClientApp"]
                        },
                        
                    },
                }]
            },
            {
                test: /skin\.css$/i,
                use: [MiniCssExtractPlugin.loader, 'css-loader'],
            },
            {
                test: /\.(png|jpe?g|gif)$/i,
                // More information here https://webpack.js.org/guides/asset-modules/
                type: "asset"
            },
            // For webpack v5
            {
                test: /\.(eot|svg|ttf|woff|woff2)$/i,
                // More information here https://webpack.js.org/guides/asset-modules/
                use: "url-loader"
            }
            
        ]
    },
    optimization: {
        splitChunks: {
            cacheGroups: {
                defaultVendors: {
                    filename: '[name].bundle.js',
                },
                vendor: {
                    test: /[\\/]node_modules[\\/](jquery|bootstrap|kendo-ui-core)[\\/]/,
                    name: 'vendor',
                    chunks: 'all',
                },
                tinymceVendor: {
                    test: /[\\/]node_modules[\\/](tinymce)[\\/](.*js|.*skin.css)|[\\/]plugins[\\/]/,
                    name: 'tinymce'
                }
            },
        }
    },
    plugins: [
        new webpack.ProvidePlugin({
            //$: 'jquery',
            //jQuery: 'jquery',
            // kendo: 'kendo-ui-core',
            // 'window.kendo': 'kendo-ui-core'
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