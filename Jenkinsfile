pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: 'https://github.com/therdean/aspnet-mock.git'
            }
        }
        stage('Build') {
            steps {
                bat 'dotnet build --configuration Release'
            }
        }
        stage('Publish') {
            steps {
                bat 'dotnet publish --configuration Release --output C:\\Users\\Dejan.Ristevski\\Desktop\\aspnet_app\\publish'
            }
        }
        stage('Backup') {
            steps {
                script {
                    def date = new Date().format('yyyyMMdd_HHmmss')
                    def backupDir = "C:\\Users\\Dejan.Ristevski\\Desktop\\aspnet_app\\backups\\backup_${date}"
                    bat "mkdir ${backupDir}"
                    bat "robocopy C:\\Users\\Dejan.Ristevski\\Desktop\\aspnet_app\\publish ${backupDir} /E /COPY:DAT"
                }
            }
        }
    }
}

// def date = new Date().format('yyyyMMdd_HHmmss')
// def backupDir = "C:\\Users\\Dejan.Ristevski\\Desktop\\aspnet_app\\backups\\backup_${date}"
// bat "mkdir ${backupDir}"
// bat "robocopy C:\\Users\\Dejan.Ristevski\\Desktop\\aspnet_app\\publish ${backupDir} /E /S /COPYALL"
