pipeline {
    agent any

    environment {
        VERSION_FILE = 'VERSION'
        NEW_VERSION = ''
        REPO_URL = 'https://github.com/therdean/aspnet-mock.git'
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: env.REPO_URL
            }
        }

        stage('Read and Increment Version') {
            steps {
                script {
                    def versionFile = readFile env.VERSION_FILE
                    def versionParts = versionFile.trim().split('\\.')
                    def major = versionParts[0].toInteger()
                    def minor = versionParts[1].toInteger()
                    def patch = versionParts[2].toInteger()

                    patch += 1

                    def newVersion = "${major}.${minor}.${patch}"

                    writeFile file: env.VERSION_FILE, text: newVersion

                    env.NEW_VERSION = newVersion
                }
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
                    bat "xcopy C:\\Users\\Dejan.Ristevski\\Desktop\\aspnet_app\\publish ${backupDir} /e /i /s"
                }
            }
        }
    }

    post {
        success {
            script {
                bat 'git config user.email "dejanristevski96@gmail.com"'
                bat 'git config user.name "therdean"'

                bat 'git add VERSION'
                bat 'git commit -m "Updated version"'

                if (env.NEW_VERSION) {
                    bat "git tag -a v${env.NEW_VERSION} -m 'Version ${env.NEW_VERSION}'"
                } else {
                    bat 'git tag -a "v9.8" -m "Version 9.8"'
                }

                withCredentials([sshUserPrivateKey(credentialsId: 'e4366e8a-e7be-413e-9c50-1901ccae74aa', keyFileVariable: 'SSH_KEY')]) {
                    env.GIT_SSH_COMMAND = "C:\\Program Files\\Git\\usr\\bin\\ssh.exe -i ${SSH_KEY} -o StrictHostKeyChecking=no"

                    bat '''
                        set GIT_SSH_COMMAND=%GIT_SSH_COMMAND%
                        git push origin main
                        git push origin --tags
                    '''
                }
            }
        }
    }
}

