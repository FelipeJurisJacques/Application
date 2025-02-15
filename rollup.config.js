// import { rollup } from 'rollup'
// import babel from '@rollup/plugin-babel'
import typescript from '@rollup/plugin-typescript'

export default {
  input: 'wwwroot/source/service-worker.ts',
  output: {
    file: 'wwwroot/dist/service-worker.js',
    format: 'es',
    sourcemap: false,
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
