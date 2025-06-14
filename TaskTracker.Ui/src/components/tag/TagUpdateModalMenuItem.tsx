import React from "react";
import { observer } from "mobx-react-lite";
import ITag from "../../interfaces/ITag";
import { useTranslation } from "../../hooks/useTranslation";
import TagUpdateModal from "./TagUpdateModal";

interface ITagUpdateModalMenuItemProps {
    tag: ITag;
}

const TagUpdateModalMenuItem: React.FC<ITagUpdateModalMenuItemProps> = observer(
    props => {
        const t = useTranslation();

        const [isModalOpen, setIsModalOpen] = React.useState(false);

        const handleUpdateTagButtonClick = () => {
            setIsModalOpen(true);
        };

        const hideTagUpdateModal = () => {
            setIsModalOpen(false);
        };

        return (
            <>
                <div onClick={handleUpdateTagButtonClick}>{t("updateTag")}</div>
                <TagUpdateModal
                    tag={props.tag}
                    isModalOpen={isModalOpen}
                    hideModal={hideTagUpdateModal}
                />
            </>
        );
    }
);

export default TagUpdateModalMenuItem;
