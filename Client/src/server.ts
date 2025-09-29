import {
  AngularNodeAppEngine,
  createNodeRequestHandler,
  isMainModule,
  writeResponseToNodeResponse,
} from '@angular/ssr/node';
import express from 'express';
import { dirname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const serverDistFolder = dirname(fileURLToPath(import.meta.url));
const browserDistFolder = resolve(serverDistFolder, '../browser');

const app = express();
const angularApp = new AngularNodeAppEngine();

/**
 * Example REST API endpoints
 */
// app.get('/api/**', (req, res) => {
//   // handle API requests here
// });

/**
 * Serve static files from /browser
 */
app.use(
  express.static(browserDistFolder, {
    maxAge: '1y',
    index: false, // do not auto-serve index.html for static
    redirect: false,
  }),
);

/**
 * Handle all other requests with Angular SSR
 * Acts as history API fallback
 */
app.get('*', (req, res, next) => {
  angularApp
    .handle(req)
    .then((response) => {
      if (response) {
        writeResponseToNodeResponse(response, res);
      } else {
        // fallback to index.html if SSR returns nothing
        res.sendFile(resolve(browserDistFolder, 'index.html'));
      }
    })
    .catch(next);
});

/**
 * Start server
 */
if (isMainModule(import.meta.url)) {
  const port = process.env['PORT'] || 4000;
  app.listen(port, () => {
    console.log(`Node Express server listening on http://localhost:${port}`);
  });
}

/**
 * Export request handler for Angular CLI
 */
export const reqHandler = createNodeRequestHandler(app);
