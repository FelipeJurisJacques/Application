// import { rollup } from 'rollup'
// import babel from '@rollup/plugin-babel'
import typescript from '@rollup/plugin-typescript'

export default {
  input: {
    'index': 'wwwroot/source/index.ts',
    'service-worker': 'wwwroot/source/service-worker.ts',
  },
  output: {
    format: 'es',
    sourcemap: false,
    dir: 'wwwroot/dist/',
    entryFileNames: (chunkInfo) => {
      return chunkInfo.name === 'index' ? 'index.mjs' : '[name].js';
    },
  },
  plugins: [
    typescript({
      tsconfig: './tsconfig.json',
    }),
    // babel({
    //   babelHelpers: 'bundled',
    // }),
  ]
}
