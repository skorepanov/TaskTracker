import React from "react";
import { observer } from "mobx-react-lite";
import { useStore } from "../stores/RootStore";
import TaskListView from "./TaskListView";

const InboxTaskList: React.FC = observer(() => {
    const { taskStore } = useStore();

    const incompletedTasks = taskStore.getInboxIncompletedTasks();

    const incompletedTaskComponents = incompletedTasks.map(t => (
        <TaskListView
            key={`inbox-${t.id}`}
            task={t}
        />
    ));

    const completedTaskComponents = taskStore
        .getInboxCompletedTasks()
        .map(t => (
            <TaskListView
                key={`inbox-${t.id}`}
                task={t}
            />
        ));

    return (
        <>
            <strong>
                Inbox (не выполнено задач: {incompletedTasks.length})
            </strong>
            {incompletedTaskComponents}
            {completedTaskComponents}
        </>
    );
});

export default InboxTaskList;
