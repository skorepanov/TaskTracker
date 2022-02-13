export default interface ITask {
    id: number | null,
    title: string,
    description: string,
    dueDate: Date | null,
    folderId: number,
}
