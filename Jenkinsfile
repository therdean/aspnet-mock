pipeline {
    agent any

    environment {
        REPO_URL = 'https://github.com/therdean/aspnet-mock.git'
        VERSION_FILE = 'VERSION'
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: env.REPO_URL
            }
        }

        stage('Determine version') {
            steps {
                script {
                    def currentVersion = readFile(env.VERSION_FILE).trim()
                    echo "Current Version: ${currentVersion}"

                    def versionParts = currentVersion.split('\\.')
                    versionParts[-1] = (versionParts[-1] as int) + 1
                    def newVersion = versionParts.join('.')
                    env.VERSION_TAG = "v${newVersion}"
                    echo "New Version: ${newVersion}"

                    writeFile file: env.VERSION_FILE, text: newVersion
                }
            }
        }

        stage('Commit Version') {
            steps {
                withCredentials([usernamePassword(credentialsId: 'github-pat', usernameVariable: 'USERNAME', passwordVariable: 'TOKEN')]) {
                    script {
                        git credentialsId: 'github-pat',
                                url: env.REPO_URL,
                                branch: 'main',
                                commitMessage: "Update version to ${env.VERSION_TAG}",
                                extensions: [[$class: 'CleanBeforeCheckout']]
                    }
                }
            }
        }

        stage('Tag Version') {
            steps {
                withCredentials([usernamePassword(credentialsId: 'github-pat', usernameVariable: 'USERNAME', passwordVariable: 'TOKEN')]) {
                    script {
                        def version = env.VERSION_TAG
                        bat "git tag ${version}"
                        bat "git push https://$env.USERNAME:$env.TOKEN@github.com/therdean/aspnet-mock.git ${version}"
                    }
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
                    def version = env.VERSION_TAG
                    // def time = new Date().format('HH-mm')
                    // def backupDir = "C:\\Users\\Dejan.Ristevski\\Desktop\\aspnet_app\\backups\\backup_${time}"
                    def backupDir = "C:\\Users\\Dejan.Ristevski\\Desktop\\aspnet_app\\backups\\backup_v_${version}"
                    bat "mkdir ${backupDir}"
                    bat "xcopy C:\\Users\\Dejan.Ristevski\\Desktop\\aspnet_app\\publish ${backupDir} /e /i /s"
                }
            }
        }
    }
}
