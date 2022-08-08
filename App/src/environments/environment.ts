const ip = window.location.hostname;
let host = 'localhost';
if (ip === '10.4.4.224') {
  host = ip;
}
export const environment = {
  production: false,
  apiUrl: `http://${ip}:58/api/`,
  // apiUrl: `https://api-yeelong.com/api/`,
  versionCheckURL : '/assets/version.json',
  domain: `http://10.4.5.174:58`
};
