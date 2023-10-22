require('dotenv').config();

const host = process.env.MYSQL_HOST;
const port = process.env.MYSQL_PORT;
const user = process.env.MYSQL_USER;
const password = process.env.MYSQL_PASSWORD;
const database = process.env.MYSQL_DB;

module.exports = {
    host,
    port,
    user,
    password,
    database
};