#  Ly Booking
## Get started

### Install via npm

```bash
npm install --global yarn
```

# How to run the project

### Install npm packages

Install the `yarn` packages described in the `package.json` and verify that it works:

```shell
yarn install
yarn start
```
The `yarn start` command builds (compiles TypeScript and copies assets) the application into `dist/`, watches for changes to the source files, and runs `lite-server` on port `1000`.

Shut it down manually with `Ctrl-C`.

#### npm scripts

These are the most useful commands defined in `package.json`:

* `yarn start` - runs the TypeScript compiler, asset copier, and a server at the same time, all three in "watch mode".
* `yarn run prod` - runs the TypeScript compiler and asset copier in "watch mode"; when changes occur to source files, they will be recompiled or copied into `dist/`.
