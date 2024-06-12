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
                bat 'dotnet publish --configuration Release --output C:\\publish'
            }
        }
        stage('Backup') {
            steps {
                script {
                    def date = new Date().format('yyyyMMdd_HHmmss')
                    def backupDir = "C:\\backups-aspnet-core-mock\\aspnet_mock_app_backup_${date}"
                    bat "mkdir ${backupDir}"
                    bat "xcopy C:\\publish ${backupDir} /E /I"
                }
            }
        }
    }
}
