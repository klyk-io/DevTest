// rollup.config.js
import { nodeResolve } from '@rollup/plugin-node-resolve';

export default {
    input: 'ClientApp/bob.js',
    output: [{
        file: 'ClientApp/bundled.js',
        sourcemap: 'inline',
        globals: {
            jquery: '$'
        }
    }],
    external: ['jquery'],
    treeshake: false,
    plugins: [
        nodeResolve()
    ]
}